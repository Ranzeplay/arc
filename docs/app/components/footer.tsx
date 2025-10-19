export default function Footer() {
    return (
      <footer className="py-5 flex flex-row items-baseline justify-between px-12 bg-neutral-200 text-neutral-800 mt-auto">
        <p>
          Copyright &copy; 2023-{process.env.NEXT_PUBLIC_COPYRIGHT_YEAR} -
          The Arc Programming Language
        </p>
        {process.env.NEXT_PUBLIC_NODE_ENV == "production" ? (
          <p className="text-xs font-mono text-neutral-500">
            Rev: {process.env.NEXT_PUBLIC_COMMIT_HASH} @{" "}
            {process.env.NEXT_PUBLIC_BUILD_TIME}
          </p>
        ) : (
          <p className="text-xs font-mono text-neutral-500">
            Development Build
          </p>
        )}
      </footer>
    );
}
