import {auth} from "@/lib/auth";
import {headers} from "next/headers";
import {getSessionCookie} from "better-auth/cookies";

export type FetchResult<T> = {ok: true; data: T} 
    | {ok: false; status: number; error: string}

const baseUrl = process.env.BASE_API_URL;

async function getAuthHeaders(): Promise<HeadersInit> {
    try {
        const reqHeaders = await headers();
        
        if (!getSessionCookie(reqHeaders)) return {};
        
        const {accessToken} = await auth.api.getAccessToken({
            body: {providerId: 'duende'},
            headers: await headers()
        });
        
        return accessToken ? {Authorization: `Bearer ${accessToken}`} : {}
    } catch (e) {
        console.log(e);
        return {};
    }
}

export async function fetchWrapper<T>(url: string, init: RequestInit = {}): Promise<FetchResult<T>>{
    try {
        const authHeaders = await getAuthHeaders();
        
        const res = await fetch(baseUrl + url, {
            ...init,
            headers: {
                'Content-Type': 'application/json',
                ...authHeaders,
                ...init.headers
            }
        });
        
        if (!res.ok) {
            return {ok:false, status: res.status, error: res.statusText};
        }
        
        const contentType = res.headers.get('Content-Type') ?? 'application/json';
        const data = res.status === 204
            ? undefined
            : contentType.includes('application/json')
                ? await res.json()
                : await res.text()
        
        return {ok:true, data: data as T}
        
    } catch (e) {
        return {ok: false, status: 0, error: e instanceof Error 
                ? e.message : 'Something went wrong'};
    }
}