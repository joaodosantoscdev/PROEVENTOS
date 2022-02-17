import { Component, OnInit, TemplateRef } from '@angular/core';
import { Router, NavigationExtras } from '@angular/router';
import { PaginatedResult, Pagination } from '@app/models/pagination';
import { environment } from '@environments/environment';
import { BsLocaleService } from 'ngx-bootstrap/datepicker';

import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { NgxSpinnerService } from 'ngx-spinner';
import { ToastrService } from 'ngx-toastr';
import { Subject } from 'rxjs';
import { debounceTime } from 'rxjs/operators';

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
  public eventId =  0;
  public pagination = {} as Pagination;


  public widthImg = 100;
  public marginImg = 2;
  public showImg = true;

  termSearchChange: Subject<string> = new Subject<string>();

  public filterEvents(event: any): void {
    if (this.termSearchChange.observers.length === 0 ) {
      this.termSearchChange.pipe(debounceTime(1000)).subscribe(
        filterAs => {
      this.spinner.show();
      this.eventService.getEvents(
        this.pagination.currentPage,
        this.pagination.itemsPerPage,
        filterAs
        ).subscribe(
          (paginatedResult: PaginatedResult<Event[]>) => {
            this.events = paginatedResult.result;
            this.pagination = paginatedResult.pagination;
          },
          (error: any) => {
            this.spinner.hide();
            this.toastr.error('Erro ao Carregar os Eventos', 'Erro!');
          }
        ).add(() => this.spinner.hide());
      });
    }
    this.termSearchChange.next(event.value);
  }

  constructor(
    private eventService: EventService,
    private modalService: BsModalService,
    private toastr: ToastrService,
    private spinner: NgxSpinnerService,
    private router: Router,
    ) {  }

  ngOnInit(): void {
    this.loadEvents();
  }

  public changeImg(): void {
    this.showImg = !this.showImg;
  }

  public loadEvents(): void {
    this.spinner.show();

    this.eventService.getEvents(this.pagination.currentPage,
                                this.pagination.itemsPerPage).subscribe(
        (paginatedResult: PaginatedResult<Event[]>) => {
          this.events = paginatedResult.result;
          this.pagination = paginatedResult.pagination;
        },
        (error: any) => {
          this.spinner.hide();
          this.toastr.error('Erro ao Carregar os Eventos', 'Erro!');
        }
      )
      .add(() => this.spinner.hide());
  }

  openModal(event, template: TemplateRef<any>, eventId: number): void  {
    event.stopPropagation();
    this.eventId = eventId;
    this.modalRef = this.modalService.show(template, {class: 'modal-sm'});
  }

  public pageChanged(event): void {
    this.pagination.currentPage = event.page;
    this.loadEvents();
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
    // this.router.navigate([`events/details/${id}`]);
    location.assign(`events/details/${id}`);
  }

  navigateNewEvent(): void {
    location.assign('events/details');
  }

  public showImages(imageURL: string): string {
    return (imageURL !== '')
    ? `${environment.apiURL}Resources/Images/${imageURL}`
    : 'assets/img/withoutImg.jpg';
  }
}
