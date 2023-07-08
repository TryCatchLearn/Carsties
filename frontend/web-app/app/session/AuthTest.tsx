'use client'

import React, { useState } from 'react'
import { updateAuctionTest } from '../actions/auctionActions';
import { Button } from 'flowbite-react';

export default function AuthTest() {
    const [loading, setLoading] = useState(false);
    const [result, setResult] = useState<any>();

    function doUpdate() {
        setResult(undefined);
        setLoading(true);
        updateAuctionTest()
            .then(res => setResult(res))
            .finally(() => setLoading(false))
            
    }

    return (
        <div className='flex items-center gap-4'>
            <Button outline isProcessing={loading} onClick={doUpdate}>
                Test auth
            </Button>
            <div>
                {JSON.stringify(result, null, 2)}
            </div>
        </div>
    )
}
