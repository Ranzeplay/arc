"use client";

import { Toc } from "@stefanprobst/rehype-extract-toc";
import { ChevronDown } from "lucide-react";
import Link from "next/link";
import React, { useState } from "react";

export default function TableOfContents({
  toc,
  maxDepth = 3,
  className,
  hideTitle,
}: {
  toc: Toc;
  maxDepth?: number;
  className?: string;
  hideTitle?: boolean;
}) {
  const [expand, setExpand] = useState(true);

  return (
    <nav className={`text-sm ${className}`}>
      {!hideTitle && (
        <div className="flex flex-row justify-between items-center">
          <h2 className="font-bold text-lg">Table of Contents</h2>
          <button
            className="text-neutral-500 text-sm hover:underline cursor-pointer"
            onClick={() => setExpand(!expand)}
          >
            <ChevronDown className={`transition-transform ${!expand ? "rotate-180" : ""}`} />
          </button>
        </div>
      )}
      <div className={expand ? "" : "hidden"}>
        {!hideTitle && toc.length === 0 && (
          <p className="text-neutral-500 italic">Empty</p>
        )}
        <ul className="flex flex-col gap-y-1 mt-1">
          {toc.map((item, index) => (
            <li key={index}>
              <Link
                href={`#${item.id}`}
                className="text-neutral-600 hover:underline"
              >
                {item.value}
              </Link>
              {item.children && item.depth < maxDepth && (
                <TableOfContents
                  toc={item.children}
                  maxDepth={maxDepth}
                  className={`${className} ml-2`}
                  hideTitle
                />
              )}
            </li>
          ))}
        </ul>
      </div>
    </nav>
  );
}
