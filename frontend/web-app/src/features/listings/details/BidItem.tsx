import {Bid} from "@/lib/types";
import {clsx} from "clsx";
import {User} from "lucide-react";
import {formatDateTime, splitPascalCase, usdFormatter} from "@/lib/utils";

type Props = {
    bid: Bid
}

export default function BidItem({ bid }: Props) {
    return (
        <div className={clsx('flex min-h-16 items-center rounded-lg justify-between gap-4 border-border/50 px-4 last:border-b-0', {
            'bg-green-200': bid.bidStatus === 'Accepted',
            'bg-amber-200': bid.bidStatus === 'AcceptedBelowReserve',
            'bg-red-200': bid.bidStatus === 'TooLow',
            'bg-red-300': bid.bidStatus === 'Finished',
        })}>
            <div className='flex flex-col justify-start'>
                <div className='flex items-center gap-1 capitalize text-lg'>
                    <User className='h-5 w-5' />
                    {bid.bidder}
                </div>
                <p className='text-sm text-muted-foreground'>
                    {formatDateTime(bid.bidTime)}
                </p>
            </div>
            <div className='flex flex-col justify-end gap-2'>
                <p className='font-semibold self-end'>
                    {usdFormatter.format(bid.amount)}
                </p>
                <p>
                    {splitPascalCase(bid.bidStatus)}
                </p>
            </div>
        </div>
    );
}