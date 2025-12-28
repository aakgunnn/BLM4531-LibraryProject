-- Fix DueDate column to be nullable
ALTER TABLE "Loans" ALTER COLUMN "DueDate" DROP NOT NULL;
