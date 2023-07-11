export type PagedResult<T> = {
    results: T[]
    pageCount: number
    totalCount: number
}

export type Auction = {
    reservePrice: number
    seller: string
    winner?: string
    soldAmount: number
    currentHighBid: number
    createdAt: string
    updatedAt: string
    auctionEnd: string
    status: string
    make: string
    model: string
    year: number
    color: string
    mileage: number
    imageUrl: string
    id: string
  }
  
  export type Bid = {
    id: string
    auctionId: string
    bidder: string
    bidTime: string
    amount: number
    bidStatus: string
  }

  export type AuctionFinished = {
    itemSold: boolean
    auctionId: string
    winner?: string
    seller: string
    amount?: number
  }