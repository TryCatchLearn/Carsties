import {DefaultSession} from 'next-auth';

declare module 'next-auth' {
    interface Session {
        user: {
            id: string
            username: string
        } & DefaultSession['user']
    }

    interface Profile {
        username: string
    }

    interface User {
        username: string
    }
}

declare module 'next-auth/jwt' {
    interface JWT {
        username: string
        access_token?: string
    }
}