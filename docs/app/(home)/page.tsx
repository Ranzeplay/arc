import { Activity, Book, Code, Scale, Users } from "lucide-react";
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

export default function HomePage() {
  return (
    <main className={`flex flex-1 flex-col my-8 lg:justify-center container px-4 md:px-6 lg:px-8 ${bodyFont.className}`}>
      <h2 className="mb-2 text-lg md:text-xl">The documentation site of </h2>
      <h1 className={`mb-6 md:mb-8 text-3xl md:text-4xl lg:text-5xl font-bold ${titleFont.className}`}>The Arc Programming Language</h1>
      <div className="flex flex-col sm:flex-row gap-4 sm:gap-x-4">
        <Link href="/docs" className="btn btn-primary flex flex-row gap-x-2 items-center justify-center">
          <Book size={20} />
          <span>Documentation</span>
        </Link>
        <Link
          href="https://github.com/Ranzeplay/arc"
          className="btn btn-secondary flex flex-row gap-x-2 items-center justify-center"
        >
          <Code size={20} />
          <span>Source</span>
        </Link>
      </div>
      <div className="mt-8 flex flex-col lg:flex-row w-full justify-center gap-4">
        <div className="card flex-1">
          <div className="card-header">
            <div className="card-icon">
              <Activity size={20} className="text-white" />
            </div>
            <h3 className={`card-title ${titleFont.className}`}>Early Development</h3>
          </div>
          <p className="card-content">
            Arc is currently in early development, with many features still being implemented.
            The language is designed to be expressive and precise.
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
            The project is licensed under the MIT License, allowing for free use and modification.
          </p>
        </div>
        <div className="card flex-1">
          <div className="card-header">
            <div className="card-icon">
              <Users size={20} className="text-white" />
            </div>
            <h3 className={`card-title ${titleFont.className}`}>Contribution</h3>
          </div>
          <p className="card-content">
            We need your help!
            If you are interested in contributing to the project,
            please check out our GitHub repository.
            Feel free to open issues, submit pull requests, or just give us feedback.
          </p>
        </div>
      </div>
    </main>
  );
}

export const metadata = {
  title: "Arc Programming Language",
  description: "The documentation site for the Arc programming language.",
};
