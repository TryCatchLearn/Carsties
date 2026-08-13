import {Card, CardContent, CardDescription, CardHeader, CardTitle} from "@/components/ui/card";
import {Auction, Bid} from "@/lib/types";
import BidForm from "@/features/listings/details/BidForm";
import BidHistory from "@/features/listings/details/BidHistory";
import {getCurrentUser} from "@/lib/auth";
import {usdFormatter} from "@/lib/utils";
import {Alert, AlertTitle} from "@/components/ui/alert";
import {Gavel} from "lucide-react";
import BidPanelContent from "@/features/listings/details/BidPanelContent";

type Props = {
    bids: Bid[]
    auction: Auction
}

export default async function BidPanel({bids, auction}: Props) {
    const user = await getCurrentUser();
    const isSeller = user?.username === auction.seller;
    const highBid = bids.reduce((prev, current) => {
        return prev > current.amount ? prev : current.amount;
    }, 0);
    const isSold = auction.currentHighBid > auction.reservePrice;

    return (
        <BidPanelContent
            bids={bids}
            auction={auction}
            isSeller={isSeller}
            initialHighBid={highBid}
            isSold={isSold}
            isLoggedIn={!!user}
        />
    );
}