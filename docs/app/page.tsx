import { Activity, Book, Code, Power, Scale, Split, Users } from "lucide-react";
import { Encode_Sans, Noto_Sans } from "next/font/google";
import Link from "next/link";

const bodyFont = Noto_Sans({
  subsets: ["latin"],
  weight: ["400", "700"],
});

const titleFont = Encode_Sans({
  subsets: ["latin"],
  weight: ["400", "700"],
});

export const metadata = {
  title: "The Arc Programming Language",
  description: "The documentation site for the Arc programming language.",
};

export default function Home() {
  return (
    <main
      className={`flex flex-1 flex-col my-8 lg:justify-center container px-4 md:px-6 lg:px-8 h-full mx-auto gap-4 ${bodyFont.className}`}
    >
      <div className="flex flex-col">
        <h2 className="mb-2 text-lg md:text-xl">The documentation site of </h2>
        <h1
          className={`mb-2 md:mb-4 text-3xl md:text-4xl lg:text-5xl font-bold ${titleFont.className}`}
        >
          The Arc Programming Language
        </h1>
        <div className="flex flex-col sm:flex-row gap-4 sm:gap-x-4">
          <Link
            href="/usage"
            className="btn btn-primary flex flex-row gap-x-2 items-center justify-center"
          >
            <Power size={20} />
            <span>Getting Started</span>
          </Link>
          <Link
            href="https://github.com/Ranzeplay/arc"
            className="btn btn-secondary flex flex-row gap-x-2 items-center justify-center"
          >
            <Code size={20} />
            <span>Source Code</span>
          </Link>
        </div>
      </div>
      <div className="flex flex-col lg:flex-row w-full justify-center gap-4">
        <div className="card flex-1">
          <div className="card-header">
            <div className="card-icon">
              <Activity size={20} className="text-white" />
            </div>
            <h3 className={`card-title ${titleFont.className}`}>
              Early Development
            </h3>
          </div>
          <p className="card-content">
            Arc is currently in early development, with many features still
            being implemented. The language is designed to be expressive and
            precise.
          </p>
        </div>
        <div className="card flex-1">
          <div className="card-header">
            <div className="card-icon">
              <Scale size={20} className="text-white" />
            </div>
            <h3 className={`card-title ${titleFont.className}`}>Open Source</h3>
          </div>
          <p className="card-content">
            The project is licensed under the MIT License, allowing for free use
            and edit. Check out the GitHub repository for more details.
          </p>
        </div>
        <div className="card flex-1">
          <div className="card-header">
            <div className="card-icon">
              <Users size={20} className="text-white" />
            </div>
            <h3 className={`card-title ${titleFont.className}`}>
              Contribution
            </h3>
          </div>
          <p className="card-content">
            We need your help! If you are interested in contributing to the
            project, please check out our GitHub repository. Feel free to open
            issues, submit pull requests, or just give us feedback.
          </p>
        </div>
      </div>
      <div className="bg-orange-200 outline outline-orange-500/50 rounded-md p-3 flex flex-row gap-x-2 items-center mt-4 shadow-md">
        <Split size={16} className="text-orange-800 font-bold" />
        <p className="text-orange-900">
          We are currently overhauling the instruction set,
          breaking changes may happen frequently.
          Please check <Link href="/ginkgo" className="underline hover:text-orange-900 font-bold">here</Link> changelog for more details.
        </p>
      </div>
    </main>
  );
}
