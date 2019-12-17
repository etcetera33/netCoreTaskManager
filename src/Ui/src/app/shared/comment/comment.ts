import { User } from './../user/user';
export class Comment {
    Id: number;
    Body: string;
    SentAt: Date;
    Author: User;
}
