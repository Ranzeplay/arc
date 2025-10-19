import { PageMetaProps } from "./pageMeta";

export interface DirectoryMetaProps {
  title: string;
  description?: string;
  content: (DirectoryMetaProps | PageMetaProps)[]
}
