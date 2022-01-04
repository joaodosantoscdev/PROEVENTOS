import { Event } from "./Event";
import { SocialMedia } from "./SocialMedia";

export interface Speaker {
  id: number,
  name: string,
  cV: string,
  imageURL: string,
  callNumber: string,
  email: string,
  socialMedias: SocialMedia[],
  speakerEvents: Event[],
}
