import { Component, OnInit, TemplateRef } from '@angular/core';
import { Router } from '@angular/router';
import { environment } from '@environments/environment';
import { BsLocaleService } from 'ngx-bootstrap/datepicker';

import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { NgxSpinnerService } from 'ngx-spinner';
import { ToastrService } from 'ngx-toastr';

import { Event } from 'src/app/models/Event';
import { EventService } from 'src/app/services/event.service';

@Component({
  selector: 'app-event-list',
  templateUrl: './event-list.component.html',
  styleUrls: ['./event-list.component.scss']
})
export class EventListComponent implements OnInit {
  modalRef?: BsModalRef;
  public events: Event[] = [];
  public eventsFilter: Event[] = [];
  public eventId =  0;

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

  constructor(
    private eventService: EventService,
    private modalService: BsModalService,
    private toastr: ToastrService,
    private spinner: NgxSpinnerService,
    private router: Router,
    ) {  }

  ngOnInit(): void {
    this.spinner.show();
    this.loadEvents();
  }

  public changeImg(): void {
    this.showImg = !this.showImg;
  }

  public loadEvents(): void {
    this.eventService.getEvents().subscribe({
      next: (response: Event[]) => {
        this.events = response;
        this.eventsFilter = this.events;
      },
      error: (error: any) => {
        this.spinner.hide();
        this.toastr.error('Erro ao carregar os eventos.', 'Erro!');
        console.log(error);
      },
      complete: () => this.spinner.hide()
    });
  }

  openModal(event, template: TemplateRef<any>, eventId: number): void  {
    event.stopPropagation();
    this.eventId = eventId;
    this.modalRef = this.modalService.show(template, {class: 'modal-sm'});
  }

  confirm(): void {
    this.modalRef?.hide();
    this.spinner.show();

    this.eventService.deleteEvent(this.eventId).subscribe(
      (result: any) => {
        this.toastr.success('O Evento foi deletado com sucesso', 'Deletado!');
        this.loadEvents();
      },
      (error: any) => {
        this.toastr.error(`Erro ao tentar deletar o evento ${this.eventId}`);
        console.error(error);
    }
    ).add(() => this.spinner.hide());
  }

  decline(): void {
    this.modalRef?.hide();
  }

  detailsEvent(id: number): void {
    this.router.navigate([`events/details/${id}`]);
  }

  public showImages(imageURL: string): string {
    return (imageURL !== '')
    ? `${environment.apiURL}Resources/Images/${imageURL}`
    : 'assets/img/withoutImg.jpg';
  }
}
