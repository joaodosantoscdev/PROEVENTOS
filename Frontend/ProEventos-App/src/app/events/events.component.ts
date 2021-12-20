import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-events',
  templateUrl: './events.component.html',
  styleUrls: ['./events.component.scss']
})
export class EventsComponent implements OnInit {

  public events: any = [];
  public eventsFilter: any = [];
  widthImg = 100;
  marginImg = 2;
  showImg = true;
  private FilterList = '';

  public get filterList(): string {
    return this.FilterList;
  }

  public set filterList(value: string) {
    this.FilterList = value;
    this.eventsFilter = this.FilterList ? this.filterEvents(this.FilterList) : this.events;
  }

  filterEvents(filter: string): any {
    filter = filter.toLocaleLowerCase();
    return this.events.filter(
      (event: any) => event.theme.toLocaleLowerCase().indexOf(filter) !== - 1 ||
      event.local.toLocaleLowerCase().indexOf(filter) !== -1
    );
  }

  constructor(private http: HttpClient) { }

  ngOnInit(): void {
    this.getEvents();
  }

  changeImg(): void {
    this.showImg = !this.showImg;
  }

  public getEvents(): void {
    this.http.get('https://localhost:44388/api/Event').subscribe(
      response => {
        this.events = response;
        this.eventsFilter = this.events;
      },
      error => console.log(error)
    );
  }
}
