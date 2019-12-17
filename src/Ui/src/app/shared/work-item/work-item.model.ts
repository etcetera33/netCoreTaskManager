import { User } from './../user/user';
export class WorkItem {
    Id: number;
    Title: string;
    Description: string;
    Assignee: User;
    Priority: number;
    Progress: number;
    ItemType: string;
}
