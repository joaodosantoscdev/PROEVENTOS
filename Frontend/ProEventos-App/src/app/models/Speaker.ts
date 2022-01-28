import { Event } from './Event';
import { UserUpdate } from './identity/userUpdate';
import { SocialMedia } from './SocialMedia';

export interface Speaker {
  id: number;
  cv: string;
  user: UserUpdate;
  socialMedias: SocialMedia[];
  speakerEvents: Event[];
}
