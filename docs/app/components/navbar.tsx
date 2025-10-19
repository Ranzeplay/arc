import Link from "next/link";
import { Bug, FolderGit2 } from "lucide-react";

export default function NavBar() {
  return (
    <header className="items-center justify-center w-full fixed top-0 z-50 border-b border-b-stone-200/70">
      <div className="h-14 w-full backdrop-blur-md bg-neutral-50/30">
        <div className="flex flex-row items-center justify-between mx-auto w-full h-full px-6">
          <div className="flex grow items-center gap-x-5 w-full translate-y-0.5 text-sm">
            <Link href="/" className="font-semibold font-serif text-xl text-black">
              Arc
            </Link>

            <Link
              href="/usage"
              className="hover:underline text-neutral-600 hover:text-neutral-900 transition-all"
            >
              Usage
            </Link>
            <Link
              href="/reference"
              className="hover:underline text-neutral-600 hover:text-neutral-900 transition-all"
            >
              Reference
            </Link>
            <Link
              href="/stdlib"
              className="hover:underline text-neutral-600 hover:text-neutral-900 transition-all"
            >
              Stdlib
            </Link>
            <Link
              href="/article"
              className="hover:underline text-neutral-600 hover:text-neutral-900 transition-all"
            >
              Articles
            </Link>
            <Link
              href="/development"
              className="hover:underline text-neutral-600 hover:text-neutral-900 transition-all"
            >
              Development
            </Link>
          </div>
          <div className="flex items-center gap-x-5 translate-y-0.5">
            <Link
              href="https://github.com/Ranzeplay/arc"
              className="text-neutral-500 hover:text-neutral-900 transition-all"
              target="_blank"
            >
              <FolderGit2 size={20} />
            </Link>
            <Link
              href="https://github.com/Ranzeplay/arc/issues"
              className="text-neutral-500 hover:text-neutral-900 transition-all"
              target="_blank"
            >
              <Bug size={20} />
            </Link>
          </div>
        </div>
      </div>
    </header>
  );
}
