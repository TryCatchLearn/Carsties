import EmptyFilter from '@/app/components/EmptyFilter'
import React from 'react'

export default function Page({searchParams}: {searchParams: {callbackUrl: string}}) {
  return (
    <EmptyFilter 
        title='You need to be logged in to do that'
        subtitle='Please click below to sign in'
        showLogin
        callbackUrl={searchParams.callbackUrl}
    />
  )
}
