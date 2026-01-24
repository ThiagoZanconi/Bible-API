CREATE TABLE users (
  id INT GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
  name TEXT NOT NULL,
  email TEXT UNIQUE NOT NULL,
  password_hash TEXT NOT NULL,
  role TEXT NOT NULL,
  created_at TIMESTAMPTZ DEFAULT now()
);

CREATE TABLE collections (
  id INT GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
  user_id INT NOT NULL REFERENCES users(id) ON DELETE CASCADE,
  name TEXT NOT NULL,
  created_at TIMESTAMPTZ DEFAULT now(),
  UNIQUE (user_id, name)
);

CREATE TABLE translations (
  id TEXT NOT NULL,
  name TEXT NOT NULL,
  language TEXT NOT NULL,
  language_code TEXT NOT NULL,
  license TEXT,
  PRIMARY KEY (id)
);

CREATE TABLE books (
  id TEXT NOT NULL,
  name TEXT NOT NULL,
  translation_id TEXT NOT NULL,
  PRIMARY KEY (id, translation_id),
  FOREIGN KEY (translation_id)
    REFERENCES translations(id)
    ON DELETE CASCADE
);

CREATE TABLE chapters (
  book_id TEXT NOT NULL,
  chapter INT NOT NULL,
  translation_id TEXT NOT NULL,
  PRIMARY KEY (book_id, translation_id, chapter),
  FOREIGN KEY (book_id, translation_id)
    REFERENCES books(id, translation_id)
    ON DELETE CASCADE
);

CREATE TABLE verses (
  book_id TEXT NOT NULL,
  chapter INT NOT NULL,
  verse INT NOT NULL,
  translation_id TEXT NOT NULL,
  text TEXT NOT NULL,
  PRIMARY KEY (book_id, chapter, verse, translation_id),
  FOREIGN KEY (book_id, chapter, translation_id)
    REFERENCES chapters(book_id, chapter, translation_id)
    ON DELETE CASCADE
);

CREATE TABLE verse_collections (
  collection_id INT NOT NULL REFERENCES collections(id) ON DELETE CASCADE,
  book_id TEXT NOT NULL,
  chapter INT NOT NULL,
  verse INT NOT NULL,
  PRIMARY KEY (collection_id, book_id, chapter, verse)
);