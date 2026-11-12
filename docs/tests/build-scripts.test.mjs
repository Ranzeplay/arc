import assert from "node:assert/strict";
import { readFileSync } from "node:fs";
import test from "node:test";

test("dev and build generate the parser without relying on lifecycle hooks", () => {
  const { scripts } = JSON.parse(
    readFileSync(new URL("../package.json", import.meta.url), "utf8"),
  );
  for (const command of ["dev", "build"]) {
    assert.match(
      scripts[command],
      new RegExp(`^pnpm generate:grammar && next ${command}\\b`),
    );
    assert.equal(scripts[`pre${command}`], undefined);
  }
});
