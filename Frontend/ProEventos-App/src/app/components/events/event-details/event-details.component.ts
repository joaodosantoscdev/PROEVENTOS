import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';

@Component({
  selector: 'app-event-details',
  templateUrl: './event-details.component.html',
  styleUrls: ['./event-details.component.scss']
})
export class EventDetailsComponent implements OnInit {

  form: FormGroup;

  get f(): any {
    return this.form.controls;
  }

  constructor(private fb: FormBuilder) { }

  ngOnInit(): void {
    this.validation();
  }

  public validation(): void {
    this.form = this.fb.group ({
      theme: ['', [Validators.required, Validators.minLength(4), Validators.maxLength(50)]],
      local: ['', Validators.required],
      dateEvent: ['', Validators.required],
      capacity: ['', [Validators.required, Validators.max(12000), Validators.min(20)]],
      callNumber: ['', Validators.required],
      email: ['', [Validators.required, Validators.email ]],
      imageURL: ['', Validators.required],
    });
  }

  public resetForm(): void {
    this.form.reset();
  }

}
