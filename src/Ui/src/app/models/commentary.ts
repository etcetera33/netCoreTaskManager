import { User } from './user';
export class Commentary {
    Id: number;
    Body: string;
    SentAt: Date;
    Author: User;
    AuthorId: number;
    WorkItemId: number;
}
