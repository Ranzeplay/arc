"use client";

import type { Toc } from "@stefanprobst/rehype-extract-toc";
import type { MouseEvent as ReactMouseEvent } from "react";
import { useCallback, useEffect, useRef, useState } from "react";
import type { TreeNode } from "@/app/[...slug]/utils";
import Directory from "@/app/components/directory";
import TableOfContents from "@/app/components/toc";

type ResizableSidebarProps = {
  toc: Toc;
  rootNode: TreeNode;
  currentPath: string;
};

const MIN_SIDEBAR_WIDTH = 224; // 14rem
const MAX_SIDEBAR_WIDTH = 576; // 36rem
const MIN_TOC_HEIGHT = 96; // 6rem

function clamp(value: number, min: number, max: number) {
  return Math.min(Math.max(value, min), max);
}

export default function ResizableSidebar({
  toc,
  rootNode,
  currentPath,
}: ResizableSidebarProps) {
  const sidebarRef = useRef<HTMLDivElement>(null);
  const dragState = useRef<{
    type: "sidebar" | "toc" | null;
    startX: number;
    startY: number;
    startWidth: number;
    startHeight: number;
  }>({
    type: null,
    startX: 0,
    startY: 0,
    startWidth: 0,
    startHeight: 0,
  });

  const [sidebarWidth, setSidebarWidth] = useState(320);
  const [tocHeight, setTocHeight] = useState(240);

  const handleMouseMove = useCallback((event: MouseEvent) => {
    if (dragState.current.type === "sidebar") {
      const nextWidth = clamp(
        dragState.current.startWidth + (event.clientX - dragState.current.startX),
        MIN_SIDEBAR_WIDTH,
        MAX_SIDEBAR_WIDTH,
      );
      setSidebarWidth(nextWidth);
      return;
    }

    if (dragState.current.type === "toc") {
      const sidebarHeight = sidebarRef.current?.clientHeight ?? 0;
      const maxTocHeight = Math.max(MIN_TOC_HEIGHT, Math.floor(sidebarHeight * 0.7));
      const nextHeight = clamp(
        dragState.current.startHeight + (event.clientY - dragState.current.startY),
        MIN_TOC_HEIGHT,
        maxTocHeight,
      );
      setTocHeight(nextHeight);
    }
  }, []);

  const handleMouseUp = useCallback(() => {
    dragState.current.type = null;
    window.removeEventListener("mousemove", handleMouseMove);
    window.removeEventListener("mouseup", handleMouseUp);
  }, [handleMouseMove]);

  const startResize = useCallback(
    (event: ReactMouseEvent<HTMLDivElement>, type: "sidebar" | "toc") => {
      event.preventDefault();
      event.stopPropagation();

      dragState.current = {
        type,
        startX: event.clientX,
        startY: event.clientY,
        startWidth: sidebarWidth,
        startHeight: tocHeight,
      };

      window.addEventListener("mousemove", handleMouseMove);
      window.addEventListener("mouseup", handleMouseUp);
    },
    [handleMouseMove, handleMouseUp, sidebarWidth, tocHeight],
  );

  useEffect(() => {
    return () => {
      window.removeEventListener("mousemove", handleMouseMove);
      window.removeEventListener("mouseup", handleMouseUp);
    };
  }, [handleMouseMove, handleMouseUp]);

  return (
    <div
      ref={sidebarRef}
      className="sticky top-14 h-[calc(100vh-3.5rem)] p-8 shadow flex flex-col gap-y-5 overflow-hidden relative flex-none"
      style={{ width: sidebarWidth }}
    >
      <div
        className="overflow-y-auto"
        style={{ height: tocHeight, minHeight: `${MIN_TOC_HEIGHT}px` }}
      >
        <TableOfContents toc={toc} />
      </div>
      <div className="relative">
        <div
          className="w-full h-px bg-neutral-300 dark:bg-neutral-700 cursor-row-resize"
          onMouseDown={(event) => startResize(event, "toc")}
        />
        <div
          className="absolute inset-x-0 -top-2 h-4 cursor-row-resize"
          onMouseDown={(event) => startResize(event, "toc")}
        />
      </div>
      <div className="flex-1 overflow-y-auto">
        <Directory node={rootNode} currentPath={currentPath} />
      </div>
      <div
        className="absolute -right-1 top-0 h-full w-1 cursor-col-resize bg-neutral-200/70 dark:bg-neutral-800/70"
        onMouseDown={(event) => startResize(event, "sidebar")}
      />
    </div>
  );
}
