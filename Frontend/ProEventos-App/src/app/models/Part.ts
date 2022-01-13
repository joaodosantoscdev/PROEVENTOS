export interface Part {
  id: number;
  name: string;
  price: number;
  dateInitial?: Date;
  dateEnd?: Date;
  quantity: number;
  eventId: number;
  event: Event[];
}
