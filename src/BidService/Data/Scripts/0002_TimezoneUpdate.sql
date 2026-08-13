ALTER TABLE auctions
ALTER COLUMN auctionend TYPE timestamptz USING auctionend AT TIME ZONE 'UTC';

ALTER TABLE bids
ALTER COLUMN bidtime TYPE timestamptz USING bidtime AT TIME ZONE 'UTC';