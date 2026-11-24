import { FileCode2 } from "lucide-react";
import { filename } from "@/lib/output";
import { Select } from "./Setup";
import { CodeView } from "./CodeView";
import "./SourcePreview.css";

export default function SourcePreview({
  editorTheme,
  sources,
  path,
  setPath,
  text,
  loading,
  line,
}: {
  editorTheme: string;
  sources: string[];
  path: string;
  setPath: (path: string) => void;
  text: string;
  loading: boolean;
  line?: number;
}) {
  return (
    <section className="source-panel">
      <div className="panel-heading">
        <span className="panel-title">
          <FileCode2 size={15} />
          Source preview
        </span>
        <Select
          aria-label="Preview source file"
          value={path}
          onChange={(event) => setPath(event.target.value)}
        >
          {!sources.length && <option value="">Select a source file</option>}
          {[...new Set([...sources, ...(path ? [path] : [])])].map((source) => (
            <option key={source} value={source}>
              {filename(source)}
            </option>
          ))}
        </Select>
        <span className="muted">{loading ? "Loading…" : "Read only"}</span>
      </div>
      <div className="source-editor">
        {path ? (
          <CodeView
            value={text}
            language="arc"
            theme={editorTheme}
            wrap
            revealLine={line}
          />
        ) : (
          <div className="source-placeholder">
            <FileCode2 size={22} />
            <span>Choose a source file to inspect its code.</span>
          </div>
        )}
      </div>
      <div className="source-path" title={path}>
        {path || "No source selected"}
      </div>
    </section>
  );
}
