-- PostgreSQL script to add cover images to existing books
-- Run this in your PostgreSQL database

UPDATE "Books" SET "ImageUrl" = '/images/books/crime-and-punishment.png' WHERE "Title" = 'Suç ve Ceza';
UPDATE "Books" SET "ImageUrl" = '/images/books/war-and-peace.png' WHERE "Title" = 'Savaş ve Barış';
UPDATE "Books" SET "ImageUrl" = '/images/books/kurk-mantolu-madonna.png' WHERE "Title" = 'Kürk Mantolu Madonna';
UPDATE "Books" SET "ImageUrl" = '/images/books/foundation.png' WHERE "Title" = 'Foundation';
UPDATE "Books" SET "ImageUrl" = '/images/books/dune.jpg' WHERE "Title" = 'Dune';
UPDATE "Books" SET "ImageUrl" = '/images/books/1984.jpg' WHERE "Title" = '1984';
UPDATE "Books" SET "ImageUrl" = '/images/books/sapiens.jpg' WHERE "Title" = 'Sapiens';
UPDATE "Books" SET "ImageUrl" = '/images/books/nutuk.jpg' WHERE "Title" = 'Nutuk';
UPDATE "Books" SET "ImageUrl" = '/images/books/steve-jobs.jpg' WHERE "Title" = 'Steve Jobs';
UPDATE "Books" SET "ImageUrl" = '/images/books/elon-musk.jpg' WHERE "Title" = 'Elon Musk';
UPDATE "Books" SET "ImageUrl" = '/images/books/clean-code.jpg' WHERE "Title" = 'Clean Code';
UPDATE "Books" SET "ImageUrl" = '/images/books/pragmatic-programmer.jpg' WHERE "Title" = 'The Pragmatic Programmer';
UPDATE "Books" SET "ImageUrl" = '/images/books/sophies-world.jpg' WHERE "Title" = 'Sofinin Dünyası';
UPDATE "Books" SET "ImageUrl" = '/images/books/mans-search-for-meaning.jpg' WHERE "Title" = 'İnsan Arayışı';
UPDATE "Books" SET "ImageUrl" = '/images/books/blink.jpg' WHERE "Title" = 'Blink';

-- Verify the updates
SELECT "Id", "Title", "ImageUrl" FROM "Books" WHERE "ImageUrl" IS NOT NULL;
