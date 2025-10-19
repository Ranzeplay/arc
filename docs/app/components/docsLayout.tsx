export default function Layout({
  children
}: Readonly<{
  children: React.ReactNode;
}>) {
  return (
    <main className="flex flex-row divide-x-1 divide-neutral-200 flex-1 h-full *:p-8">
      <div className="basis-1/5">
        <h2 className="font-bold text-lg">Directory</h2>
      </div>
      <div className="prose">{children}</div>
    </main>
  );
}
