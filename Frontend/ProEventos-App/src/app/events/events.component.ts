import { Component, OnInit } from '@angular/core';
import { EventService } from './../services/event.service';


@Component({
  selector: 'app-events',
  templateUrl: './events.component.html',
  styleUrls: ['./events.component.scss']
})
export class EventsComponent implements OnInit {

  public events: Event[] = [];
  public eventsFilter: Event[] = [];
  public widthImg = 100;
  public marginImg = 2;
  public showImg = true;
  private FilterList = '';

  public get filterList(): string {
    return this.FilterList;
  }

  public set filterList(value: string) {
    this.FilterList = value;
    this.eventsFilter = this.FilterList ? this.filterEvents(this.FilterList) : this.events;
  }

  public filterEvents(filter: string): Event[] {
    filter = filter.toLocaleLowerCase();
    return this.events.filter(
      (event: any) => event.theme.toLocaleLowerCase().indexOf(filter) !== - 1 ||
      event.local.toLocaleLowerCase().indexOf(filter) !== -1
    );
  }

  constructor(private eventService: EventService) { }

  ngOnInit(): void {
    this.getEvents();
  }

  public changeImg(): void {
    this.showImg = !this.showImg;
  }

  public getEvents(): void {
    this.eventService.getEvents().subscribe(
      (response: Event[]) => {
        this.events = response;
        this.eventsFilter = this.events;
      },
      error => console.log(error)
    );
  }
}
