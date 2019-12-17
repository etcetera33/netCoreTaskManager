import { User } from './../user/user';
export class WorkItem {
    Id: number;
    Title: string;
    Description: string;
    AssigneeId: number;
    Assignee: User;
    StatusId: number;
    ProjectId: number;
    AuthorId: number;
    WorkItemTypeId: number;
    Priority: number;
    Progress: number;
    ItemType: string;
}
