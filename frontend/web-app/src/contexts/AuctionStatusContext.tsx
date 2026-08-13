'use client';

import {createContext, useContext, useState} from "react";

type AuctionStatusContextValue = {
    finished: boolean;
    setFinished: (value: boolean) => void;
}

const AuctionStatusContext = createContext<AuctionStatusContextValue | null>(null);

export default function AuctionStatusProvider({ children }: { children: React.ReactNode }) {
    const [finished, setFinished] = useState(false);
    
    return (
        <AuctionStatusContext.Provider value={{finished, setFinished}}>
            {children}
        </AuctionStatusContext.Provider>
    );
}

const noop: AuctionStatusContextValue = {
    finished: false,
    setFinished: () => {}
}

export function useAuctionStatus() {
    return useContext(AuctionStatusContext) ?? noop;
}