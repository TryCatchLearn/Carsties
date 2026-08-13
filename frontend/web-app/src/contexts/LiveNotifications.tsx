'use client';

import {useSignalR} from "@/contexts/SignalRContext";
import {useEffect} from "react";
import {Auction} from "@/lib/types";
import {toast} from "@/components/ui/toast";
import {getListingDetails} from "@/features/listings/actions";
import {usdFormatter} from "@/lib/utils";
import Link from "next/link";
import Image from "next/image";

type AuctionFinishedEvent = {
    itemSold: boolean;
    auctionId: string;
    winner?: string;
    seller?: string;
    amount?: number;
}

type AuctionToastValues = {
    auctionId: string;
    title: string;
    description: string;
    imageUrl: string;
}

function AuctionToastContent(values: AuctionToastValues) {
    const {auctionId, description, title, imageUrl} = values;
    
    return (
        <Link href={`/listings/${auctionId}`} className='flex items-center gap-3'>
            <Image 
                src={imageUrl} 
                alt='image of car'
                height={64}
                width={64}
                className='h-12 w-12 rounded object-cover shrink-0'
            />
            <span className='flex flex-col justify-center gap-1 py-2'>
                <span className='text-sm font-medium'>{title}</span>
                <span className='text-sm text-muted-foreground'>{description}</span>
            </span>
        </Link>
    )
}

export default function LiveNotifications() {
    const connection = useSignalR();
    
    useEffect(() => {
        if (!connection) return;
        
        const handleAuctionCreated = (auction: Auction) => {
            toast.add({
                title: (
                    <AuctionToastContent 
                        auctionId={auction.id} 
                        title='New auction created' 
                        description={`${auction.year} ${auction.make} ${auction.model}`} 
                        imageUrl={auction.imageUrl}
                    />
                ),
            })
        }
        
        const handleAuctionFinished = async (payload: AuctionFinishedEvent) => {
            const result = await getListingDetails(payload.auctionId);
            if (!result.ok) return;
            
            const auction = result.data;
            const message = payload.itemSold
                ? `Sold to ${payload.winner} for ${usdFormatter.format(payload.amount ?? 0)}`
                : 'Reserve not met - listing did not sell';
            
            toast.add({
                title: (
                    <AuctionToastContent
                        auctionId={auction.id}
                        title={`${auction.year} ${auction.make} ${auction.model}`}
                        description={message}
                        imageUrl={auction.imageUrl}
                    />
                ),
            })
        }
        
        connection.on('AuctionCreated', handleAuctionCreated);
        connection.on('AuctionFinished', handleAuctionFinished);
        
        return () => {
            connection.off('AuctionCreated', handleAuctionCreated);
            connection.off('AuctionFinished', handleAuctionFinished);
        }
        
    }, [connection])
    
    return null;
}