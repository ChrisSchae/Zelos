-- CreateTable
CREATE TABLE IF NOT EXISTS PUBLIC.users (
  "id" TEXT NOT NULL,
  "name" TEXT NOT NULL,
  "email" TEXT NOT NULL,

  PRIMARY KEY ("id")
);

-- Seed
INSERT INTO users (id, name, email) VALUES ('userid', 'Gopher', 'hello@gopher.com');