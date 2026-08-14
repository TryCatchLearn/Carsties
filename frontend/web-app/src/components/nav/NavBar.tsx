import {CarFront} from "lucide-react";
import Link from "next/link";
import SearchInput from "@/components/nav/SearchInput";
import LoginButton from "@/components/nav/LoginButton";
import {getCurrentUser} from "@/lib/auth";
import {UserMenu} from "@/components/nav/UserMenu";
import {buttonVariants} from "@/components/ui/button";
import {Suspense} from "react";

export default async function NavBar() {
    const user = await getCurrentUser();

    return (
        <header className='sticky top-0 z-50 p-3 bg-background items-center shadow-lg flex justify-between'>
            <Link href='/' className='flex items-center gap-2 text-3xl font-semibold text-red-500'>
                <CarFront size={50}/>
                <div>Carsties Auctions</div>
            </Link>
            <Suspense>
                <SearchInput/>
            </Suspense>
            {user ? (
                <UserMenu user={user}/>
            ) : (
                <div className='flex items-center gap-2'>
                    <LoginButton/>
                    <Link 
                        className={buttonVariants({variant: 'default', size: 'lg'})}
                        href={`${process.env.NEXT_PUBLIC_ID_URL}/Account/Register`}
                    >
                        Register
                    </Link>
                </div>
                
            )}
        </header>
    )
}