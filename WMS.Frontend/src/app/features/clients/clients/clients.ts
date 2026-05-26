import { Component, inject, OnInit, ChangeDetectorRef } from '@angular/core';

import { ClientService } from '../../../core/services/client.service';
import { Client } from '../../../core/models/client';
import { CreateClient } from '../../../core/models/create-client';


@Component({
  selector: 'app-clients',
  standalone: false,
  templateUrl: './clients.html',
  styleUrl: './clients.css',
})
export class Clients {
  private clientService = inject(ClientService);
  private cdr = inject(ChangeDetectorRef);
  
  clients: Client[] = [];
  selectedClientId = 0;
  isEditMode = false;
  
  clientData: CreateClient = {
    clientName: '',
    clientAddress: '',
    clientPhoneNumber: '',
    clientLocation: '',
    status: true
  };

  ngOnInit(): void {
    this.loadClients();
  }

  loadClients(): void {
    this.clientService.getClients().subscribe({
      next: (data) => {
        this.clients = data;
        this.cdr.detectChanges();
      }
    });
  }

  saveClient(): void {
    if(this.isEditMode){
      this.clientService.updateClient(
        this.selectedClientId,
        this.clientData
      ).subscribe({
        next: () => {
          alert('Client updated successfully!');
          this.resetForm();
          this.loadClients();
        }
      });
    }else{
      this.clientService.createClient(this.clientData).subscribe({
        next: () => {
          alert('Client created successfully!');
          this.resetForm();
          this.loadClients();
        }
      });
    }
  }

  editClient(client: Client): void {
    this.selectedClientId = client.clientId;
    this.isEditMode = true;
    
    this.clientData = {
      clientName: client.clientName,
      clientAddress: client.clientAddress,
      clientPhoneNumber: client.clientPhoneNumber,
      clientLocation: client.clientLocation,
      status: client.status
    };
  }

  cancelEdit(): void {
    this.resetForm();
  }

  resetForm(): void {
    this.selectedClientId = 0;
    this.isEditMode = false;

    this.clientData = {
      clientName: '',
      clientAddress: '',
      clientPhoneNumber: '',
      clientLocation: '',
      status: true
    };
  }

  deleteClient(id: number): void {
    if(!confirm('Are you sure you want to delete this client?')) return;
    
    this.clientService.deleteClient(id).subscribe({
      next: () => {
        this.loadClients();
      },
      error: () => {
        alert('Delete blocked due to dependencies.');
      }
    });
  }
}
