CREATE TABLE IF NOT EXISTS uploaded_files (
    id UUID PRIMARY KEY,
    original_name VARCHAR(255) NOT NULL,
    stored_path TEXT NOT NULL,
    size_in_bytes BIGINT NOT NULL,
    uploaded_at TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT (NOW() AT TIME ZONE 'UTC')
);
