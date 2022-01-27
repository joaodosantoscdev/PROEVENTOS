import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';

import { PaginatedResult, Pagination } from '@app/models/pagination';
import { Speaker } from '@app/models/Speaker';
import { SpeakerService } from '@app/services/speaker.service';
import { environment } from '@environments/environment';

import { BsModalService } from 'ngx-bootstrap/modal';
import { NgxSpinnerService } from 'ngx-spinner';
import { ToastrService } from 'ngx-toastr';

import { Subject } from 'rxjs';
import { debounceTime } from 'rxjs/operators';

@Component({
  selector: 'app-speakers-list',
  templateUrl: './speakers-list.component.html',
  styleUrls: ['./speakers-list.component.scss']
})
export class SpeakersListComponent implements OnInit {

  public speakers: Speaker[] = [];
  public speakerId = 0;
  public pagination = {} as Pagination;

  termSearchChange: Subject<string> = new Subject<string>();

  constructor(
    private speakerService: SpeakerService,
    private modalService: BsModalService,
    private toastr: ToastrService,
    private spinner: NgxSpinnerService,
    private router: Router,
  ) { }



  ngOnInit(): void {
    this.loadSpeakers();
  }

  public loadSpeakers(): void {
    this.spinner.show();

    this.speakerService.getSpeakers(this.pagination.currentPage,
                                this.pagination.itemsPerPage).subscribe(
        (paginatedResult: PaginatedResult<Speaker[]>) => {
          this.speakers = paginatedResult.result;
          this.pagination = paginatedResult.pagination;
        },
        (error: any) => {
          this.spinner.hide();
          this.toastr.error('Erro ao Carregar os Eventos', 'Erro!');
        }
      )
      .add(() => this.spinner.hide());
  }

  public filterSpeakers(speaker: any): void {
    if (this.termSearchChange.observers.length === 0 ) {
      this.termSearchChange.pipe(debounceTime(1000)).subscribe(
        filterAs => {
      this.spinner.show();
      this.speakerService.getSpeakers(
        this.pagination.currentPage,
        this.pagination.itemsPerPage,
        filterAs
        ).subscribe(
          (paginatedResult: PaginatedResult<Speaker[]>) => {
            this.speakers = paginatedResult.result;
            this.pagination = paginatedResult.pagination;
          },
          (error: any) => {
            this.spinner.hide();
            this.toastr.error('Erro ao Carregar os Eventos', 'Erro!');
          }
        ).add(() => this.spinner.hide());
      });
    }
    this.termSearchChange.next(speaker.value);
  }

  public getImg(imageName: string): string {
    if (imageName) {
      return imageName = environment.apiURL + `resources/profile/${imageName}`;
    } else {
      return 'assets/img/profile-default.png';
    }
  }
}
