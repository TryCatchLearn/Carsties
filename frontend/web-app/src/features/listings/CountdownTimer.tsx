'use client'

import {useTimer} from "react-timer-hook";
import {useState} from "react";
import {clsx} from "clsx";
import {useAuctionStatus} from "@/contexts/AuctionStatusContext";

type Props = {
    auctionEnd: string;
}

export default function CountdownTimer({auctionEnd}: Props) {
    const [expired, setExpired] = useState(auctionEnd < new Date().toISOString());
    const {setFinished} = useAuctionStatus();
    
    const {days, hours, minutes, seconds} = useTimer({
        expiryTimestamp: new Date(auctionEnd),
        onExpire: () => {
            setExpired(true);
            setFinished(true);
        },
    })
    return (
        <div className={clsx('border-2 border-white text-white py-1 px-2 rounded-lg flex justify-center bg-green-600', {
            'bg-red-600': expired,
            'bg-amber-600': days === 0 && hours < 10
        })}>
            {expired ? (
                <span>Auction finished</span>
            ) : (
                <span suppressHydrationWarning>{days}:{hours}:{minutes}:{seconds}</span>
            )}
        </div>
    );
}