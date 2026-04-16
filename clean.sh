#!/bin/bash

# Define the starting directory (use current directory or specify a path)
START_DIR="$(pwd)"

# Find and remove all 'bin' and 'obj' folders recursively
find "$START_DIR" -type d \( -name "bin" -o -name "obj" \) -exec echo "Deleting: {}" \; -exec rm -rf {} +

echo "Cleanup complete."
