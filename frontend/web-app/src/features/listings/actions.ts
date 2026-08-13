'use server'

import {Auction, Bid, PagedResult} from "@/lib/types";
import {FieldValues} from "react-hook-form";
import {fetchWrapper} from "@/lib/fetch-wrapper";
import {revalidatePath} from "next/cache";

export type ListingSearchParams = {
    pageNumber?: string | string[];
    pageSize?: string | string[];
    searchTerm?: string | string[];
    orderBy?: string | string[];
    filterBy?: string | string[];
    seller?: string;
    winner?: string;
}

export async function getListings(params: ListingSearchParams = {}) {
    const { pageNumber, pageSize, searchTerm, orderBy, filterBy, seller, winner } = params;
    
    const query = new URLSearchParams({
        pageNumber: pageNumber?.toString() || String(1),
        pageSize: pageSize?.toString() || String(8),
    });
    
    if (searchTerm) query.set("searchTerm", searchTerm.toString());
    query.set('orderBy', orderBy?.toString() || 'endingSoon');
    query.set('filterBy', filterBy?.toString() || 'live');
    
    if (winner) query.set('winner', winner);
    if (seller) query.set('seller', seller);
    
    return fetchWrapper<PagedResult<Auction>>(`/search?${query}`);
}

export async function getListingDetails(id: string) {
    return fetchWrapper<Auction>(`/auctions/${id}`);
}

export async function createListing(values: FieldValues) {
    return fetchWrapper<Auction>('/auctions', {
        method: 'POST',
        body: JSON.stringify(values),
    })
}

export async function updateListing(values: FieldValues) {
    return fetchWrapper<void>(`/auctions/${values.id}`, {
        method: 'PUT',
        body: JSON.stringify(values),
    })
}

export async function deleteListing(id: string) {
    return fetchWrapper<void>(`/auctions/${id}`, {
        method: 'DELETE',
    })
}

export async function getBidsForListing(id: string) {
    return fetchWrapper<Bid[]>(`/bids/${id}`, {
        method: 'GET'
    })
}

export async function placeBidForAuction(id: string, amount: number) {
    const result = await fetchWrapper<Bid>(`/bids?auctionId=${id}&amount=${amount}`, {
        method: 'POST',
        body: JSON.stringify({})
    });
    
    revalidatePath(`/listings/${id}`);
    
    return result;
} 