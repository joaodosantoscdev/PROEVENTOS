import { Part } from './Part';
import { SocialMedia } from './SocialMedia';
import { Speaker } from './Speaker';

export interface Event {
 id: number;
 local: string;
 dateEvent?: Date;
 theme: string;
 capacity: number;
 imageURL: string;
 callNumber: string;
 email: string;
 parts: Part[];
 socialMedias: SocialMedia[];
 speakerEvents: Speaker[];
}
