import { betterAuth } from "better-auth";
import {genericOAuth, username} from "better-auth/plugins";
import {headers} from "next/headers";

export const auth = betterAuth({
    user: {
        additionalFields: {
            username: {
                type: "string",
                required: true
            }
        }
    },
    plugins: [
        genericOAuth({
            config: [
                {
                    providerId: "duende",
                    clientId: "nextApp",
                    clientSecret: "NotASecret",
                    issuer: process.env.NEXT_PUBLIC_ID_URL,
                    authorizationUrl: process.env.NEXT_PUBLIC_ID_URL + '/connect/authorize',
                    tokenUrl: process.env.ID_URL_INTERNAL + '/connect/token',
                    userInfoUrl: process.env.ID_URL_INTERNAL + '/connect/userinfo',
                    scopes: ["openid", "profile", "auctionApp"],
                    pkce: true,
                    prompt: 'login'
                }
            ]
        }),
        username()
    ]
});

export async function getCurrentUser() {
    try {
        const session = await auth.api.getSession({
            headers: await headers()
        });
        
        if (!session) return null;
        
        return session.user;
    } catch (error) {
        console.log(error);
        return null;
    }
}

export type SessionUser = typeof auth.$Infer.Session.user;