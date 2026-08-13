'use client';

import {Field, FieldContent} from "@/components/ui/field";
import {InputGroup, InputGroupAddon, InputGroupInput} from "@/components/ui/input-group";
import {Button} from "@/components/ui/button";
import {Controller, FieldValues, useForm} from "react-hook-form";
import {placeBidForAuction} from "@/features/listings/actions";
import {toast} from "@/components/ui/toast";
import {useAuctionStatus} from "@/contexts/AuctionStatusContext";

type Props = {
    auctionId: string;
    highBid: number;
    isLoggedIn: boolean;
}

export default function BidForm({ auctionId, highBid, isLoggedIn }: Props) {
    const {finished} = useAuctionStatus();
    
    const {control, handleSubmit} = useForm({
        values: {
            amount: highBid + 100
        }
    });
    
    const onSubmit = async (data: FieldValues) => {
        if (finished) return;
        const result = await placeBidForAuction(auctionId, +data.amount);
        
        if (!result.ok) {
            toast.add({
                type: "error",
                title: result.status,
                description: result.error
            })
        }
    }
    
    return (
        <form onSubmit={handleSubmit(onSubmit)} className='flex flex-col gap-3'>
            <Controller
                name='amount'
                control={control}
                render={({field}) => (
                    <Field>
                        <FieldContent>
                            <InputGroup className='h-16 rounded-lg'>
                                <InputGroupAddon className='text-3xl'>$</InputGroupAddon>
                                <InputGroupInput 
                                    className='text-3xl!' 
                                    value={field.value}
                                    onChange={field.onChange}
                                />
                            </InputGroup>
                        </FieldContent>
                    </Field>
                )} 
            />
            <Button
                type="submit"
                className='w-full rounded-lg'
                disabled={!isLoggedIn || finished}
            >
                {finished ? 'Auction finished' : isLoggedIn ? 'Place bid' : 'Login to place a bid'}
            </Button>
        </form>
    );
}