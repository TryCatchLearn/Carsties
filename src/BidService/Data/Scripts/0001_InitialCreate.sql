CREATE TABLE IF NOT EXISTS auctions (
    id text PRIMARY KEY,
    auctionEnd timestamp without time zone NOT NULL,
    seller text NOT NULL,
    reservePrice integer NOT NULL,
    finished boolean NOT NULL default false
);

CREATE TABLE IF NOT EXISTS bids (
    id text primary key,
    auctionId text not null,
    bidder text not null,
    bidTime timestamp without time zone not null,
    amount integer not null,
    bidStatus integer not null
);

CREATE INDEX IF NOT EXISTS ix_bids_auctionId ON bids (auctionId);