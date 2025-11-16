-- PostgreSQL Veritabanı Kurulum Scripti
-- Bu dosyayı PostgreSQL'de çalıştırın

-- Veritabanını oluştur (eğer yoksa)
CREATE DATABASE "LibraryDb"
    WITH 
    OWNER = postgres
    ENCODING = 'UTF8'
    LC_COLLATE = 'Turkish_Turkey.1254'
    LC_CTYPE = 'Turkish_Turkey.1254'
    TABLESPACE = pg_default
    CONNECTION LIMIT = -1;

-- Bağlan
\c LibraryDb

-- Veritabanı hazır, migration'lar otomatik uygulanacak

