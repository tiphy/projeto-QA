#!/usr/bin/env python3
"""Low-volume concurrent HTTP check for the local AlocaSenac API."""

import argparse
import sys
import time
import urllib.error
import urllib.request
from concurrent.futures import ThreadPoolExecutor


def measure(url):
    started = time.perf_counter()
    try:
        with urllib.request.urlopen(url, timeout=10) as response:
            status = response.status
    except (OSError, urllib.error.URLError):
        status = 0
    return status, time.perf_counter() - started


def percentile(samples, percentile_value):
    ordered = sorted(samples)
    index = max(0, min(len(ordered) - 1, int((percentile_value / 100) * len(ordered) + 0.999) - 1))
    return ordered[index]


def main():
    parser = argparse.ArgumentParser(description=__doc__)
    parser.add_argument("--url", default="http://localhost:5206/api/alocacoes")
    parser.add_argument("--requests", type=int, default=500)
    parser.add_argument("--concurrency", type=int, default=10)
    args = parser.parse_args()

    if args.requests < 1 or args.concurrency < 1:
        parser.error("--requests e --concurrency devem ser maiores que zero")

    started = time.perf_counter()
    with ThreadPoolExecutor(max_workers=args.concurrency) as executor:
        results = list(executor.map(measure, [args.url] * args.requests))
    elapsed = time.perf_counter() - started

    statuses = [status for status, _ in results]
    latencies = [latency for _, latency in results]
    failures = sum(status != 200 for status in statuses)
    print(f"URL: {args.url}")
    print(f"Requisições: {args.requests}; concorrência: {args.concurrency}")
    print(f"HTTP 200: {args.requests - failures}; falhas: {failures}")
    print(f"Duração: {elapsed:.3f}s; taxa: {args.requests / elapsed:.1f} req/s")
    print(
        "Latência: "
        f"p50={percentile(latencies, 50) * 1000:.1f}ms; "
        f"p95={percentile(latencies, 95) * 1000:.1f}ms; "
        f"máxima={max(latencies) * 1000:.1f}ms"
    )
    return 0 if failures == 0 else 1


if __name__ == "__main__":
    sys.exit(main())
