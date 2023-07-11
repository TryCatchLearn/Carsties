import { Bid } from "@/types"
import { create } from "zustand"

type State = {
    bids: Bid[]
    open: boolean
}

type Actions = {
    setBids: (bids: Bid[]) => void
    addBid: (bid: Bid) => void
    setOpen: (value: boolean) => void
}

export const useBidStore = create<State & Actions>((set) => ({
    bids: [],
    open: true,

    setBids: (bids: Bid[]) => {
        set(() => ({
            bids
        }))
    },

    addBid: (bid: Bid) => {
        set((state) => ({
            bids: !state.bids.find(x => x.id === bid.id) ? [bid, ...state.bids] : [...state.bids]
        }))
    },

    setOpen: (value: boolean) => {
        set(() => ({
            open: value
        }))
    }
}))