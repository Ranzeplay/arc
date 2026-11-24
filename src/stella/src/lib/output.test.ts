import { describe, expect, it } from "vitest";
import { diagnostic, runText } from "./output";
import { defaultProfile, type Run } from "./types";

describe("CLI diagnostics", () => {
  it("reads compiler generation and syntax locations", () => {
    expect(
      diagnostic(
        "fail: Circle CLI[0] /tmp/a.script.arc [(4,0)~(4,8)]: Failed generation",
      ),
    ).toMatchObject({
      severity: "error",
      path: "/tmp/a.script.arc",
      line: 4,
      column: 1,
    });
    expect(
      diagnostic(
        "fail: Circle CLI[0] C:\\Arc\\test.script.arc (3,2): unexpected token",
      ),
    ).toMatchObject({ path: "C:\\Arc\\test.script.arc", line: 3, column: 2 });
    expect(
      diagnostic(
        "fail: Circle CLI[0] /tmp/my project/a.script.arc (3,2): unexpected token",
      ).path,
    ).toBe("/tmp/my project/a.script.arc");
  });
  it("classifies stderr log levels without treating every stderr line as an error", () => {
    expect(
      diagnostic("\u001b[32mINFO\u001b[0m === Instructions").severity,
    ).toBe("info");
    expect(diagnostic("WARN decoder warning").severity).toBe("warning");
  });
  it("uses the CLI log prefix ahead of words inside the message", () => {
    expect(
      diagnostic("info: Circle CLI[0] Compilation generated with 0 error(s)")
        .severity,
    ).toBe("info");
    expect(diagnostic("[E] missing function").severity).toBe("error");
    expect(diagnostic("[W] unsupported flag").severity).toBe("warning");
    expect(diagnostic("[D] === Raw bytes").severity).toBe("debug");
  });
  it("retains exact output unless comparison normalization is enabled", () => {
    const run: Run = {
      id: "a",
      name: "a",
      started: 0,
      status: "passed",
      stages: {
        compile: { status: "passed" },
        decode: { status: "idle" },
        execute: { status: "idle" },
      },
      directory: "/tmp/run-a",
      profile: defaultProfile({
        workspace: "",
        source: "",
        compiler: "",
        runtime: "",
      }),
      events: [
        {
          kind: "output",
          stage: "compile",
          stream: "stderr",
          status: "running",
          text: "/tmp/run-a/build.pkg.arc 12ms\n",
          code: null,
          durationMs: 0,
          time: 0,
        },
      ],
    };
    expect(runText(run, "all")).toBe("/tmp/run-a/build.pkg.arc 12ms\n");
    expect(runText(run, "all", "all", "", "all", true)).toBe(
      "<run>/build.pkg.arc <duration>\n",
    );
    expect(
      runText({ ...run, directory: "" }, "all", "all", "", "all", true),
    ).toBe("/tmp/run-a/build.pkg.arc <duration>\n");
  });
});
