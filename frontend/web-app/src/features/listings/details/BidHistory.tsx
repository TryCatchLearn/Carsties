'use client';

import {Bid} from "@/lib/types";
import BidItem from "@/features/listings/details/BidItem";
import {useEffect, useRef, useState} from "react";
import {AnimatePresence, motion} from 'motion/react'
import {useSignalR} from "@/contexts/SignalRContext";

type Props = {
    initialBids: Bid[];
    auctionId: string;
}

export default function BidHistory({initialBids, auctionId}: Props) {
    const connection = useSignalR();
    const [newBidId, setNewBidId] = useState<string | null>(null);
    const [bids, setBids] = useState(initialBids);
    const bidsRef = useRef(bids);

    useEffect(() => {
        if (!connection) return;
        
        const handleBidPlaced = (bid: Bid) => {
            if (bid.auctionId !== auctionId) return;
            if (bidsRef.current.some(b => b.id === bid.id)) return;
            setBids(prev => [bid, ...prev]);
            setNewBidId(bid.id);
        }
        
        connection.on('BidPlaced', handleBidPlaced);
        
        return () => {
            connection.off('BidPlaced', handleBidPlaced);
        }
    }, [connection, auctionId]);
    

    return (
        <div className='overflow-y-auto space-y-4 scrollbar-none max-h-[60vh]'>
            {bids.length === 0 ? (
                <p>There are no bids for this listing yet</p>
            ) : (
                <AnimatePresence initial={false}>
                    {bids.map(bid => {
                        const isNew = bid.id === newBidId;
                        return (
                            <motion.div
                                key={bid.id}
                                layout
                                initial={isNew ? {opacity: 0, x: -40} : false}
                                animate={{opacity: 1, x:0}}
                                transition={{
                                    layout: {duration: 0.4, ease: 'easeOut'},
                                    opacity: {duration: 0.3, delay: isNew ? 0.4 : 0},
                                    x: {duration: 0.3, delay: isNew ? 0.4 : 0}
                                }}
                            >
                                <BidItem bid={bid} />
                            </motion.div>

                        )

                    })}
                </AnimatePresence>
            )}
           
            
        </div>
    );
}