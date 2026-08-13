'use client';

import {Card, CardContent, CardDescription, CardHeader, CardTitle} from "@/components/ui/card";
import {usdFormatter} from "@/lib/utils";
import {Alert, AlertTitle} from "@/components/ui/alert";
import {Gavel} from "lucide-react";
import BidForm from "@/features/listings/details/BidForm";
import BidHistory from "@/features/listings/details/BidHistory";
import {Auction, Bid} from "@/lib/types";
import {useSignalR} from "@/contexts/SignalRContext";
import {useEffect, useState} from "react";

type Props = {
    bids: Bid[];
    auction: Auction;
    isSeller: boolean;
    initialHighBid: number;
    isSold: boolean;
    isLoggedIn: boolean;
}

export default function BidPanelContent({bids, auction, isSeller, isSold, 
                                            initialHighBid, isLoggedIn}: Props) {
    const connection = useSignalR();
    const [highBid, setHighBid] = useState(initialHighBid);

    useEffect(() => {
        if (!connection) return;
        
        const handleBidPlaced = (bid: Bid) => {
            if (bid.auctionId !== auction.id) return;
            if (!bid.bidStatus.includes('Accepted')) return;
            setHighBid(prev => bid.amount > prev ? bid.amount : prev);
        }
        
        connection.on('BidPlaced', handleBidPlaced);
        
        return () => {
            connection.off('BidPlaced', handleBidPlaced);
        }
    }, [auction.id, connection]);
    
    return (
        <Card className='max-h-[80vh]'>
            <CardHeader>
                <CardTitle>Bid panel</CardTitle>
                <CardDescription>Minimum next bid is{' '}
                    <span className='font-bold text-foreground'>
                    {usdFormatter.format(highBid + 100)}
                </span></CardDescription>
            </CardHeader>
            <CardContent className='space-y-4 px-5 pb-5'>
                {isSeller ? (
                    <Alert>
                        <Gavel />
                        <AlertTitle>
                            The item is currently {isSold ? 'sold' : 'unsold'}
                        </AlertTitle>
                    </Alert>
                ) : (
                    <BidForm
                        auctionId={auction.id}
                        highBid={highBid}
                        isLoggedIn={isLoggedIn}
                    />
                )}
                <BidHistory initialBids={bids} auctionId={auction.id} />
            </CardContent>
        </Card>
    );
}