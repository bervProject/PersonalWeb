import React from 'react';
import {
  List,
  Datagrid,
  TextField,
  DateField,
  BooleanField,
  Create,
  SimpleForm,
  TextInput,
  DateInput,
  BooleanInput,
  Edit,
  Show,
  SimpleShowLayout,
  EditButton,
  ShowButton
} from 'react-admin';

export const ExperienceList = (props: object) => (
  <List {...props}>
    <Datagrid>
      <TextField source="id" />
      <TextField source="companyName" />
      <TextField source="roleTitle" />
      <DateField source="startDate" />
      <DateField source="endDate" />
      <BooleanField source="isCurrent" />
      <EditButton />
      <ShowButton />
    </Datagrid>
  </List>
);

export const ExperienceShow = (props: object) => (
  <Show {...props}>
    <SimpleShowLayout>
      <TextField source="id" />
      <TextField source="companyName" />
      <TextField source="roleTitle" />
      <TextField source="location" />
      <DateField source="startDate" />
      <DateField source="endDate" />
      <BooleanField source="isCurrent" />
      <TextField source="descriptionHtml" />
      <TextField source="technologiesUsed" />
    </SimpleShowLayout>
  </Show>
);

export const ExperienceCreate = (props: object) => (
  <Create {...props}>
    <SimpleForm>
      <TextInput source="companyName" />
      <TextInput source="roleTitle" />
      <TextInput source="location" />
      <DateInput source="startDate" />
      <DateInput source="endDate" />
      <BooleanInput source="isCurrent" />
      <TextInput multiline source="descriptionHtml" />
      <TextInput source="technologiesUsed" helperText='JSON array e.g. ["TypeScript","React"]' />
    </SimpleForm>
  </Create>
);

export const ExperienceEdit = (props: object) => (
  <Edit {...props}>
    <SimpleForm>
      <TextInput disabled source="id" />
      <TextInput source="companyName" />
      <TextInput source="roleTitle" />
      <TextInput source="location" />
      <DateInput source="startDate" />
      <DateInput source="endDate" />
      <BooleanInput source="isCurrent" />
      <TextInput multiline source="descriptionHtml" />
      <TextInput source="technologiesUsed" helperText='JSON array e.g. ["TypeScript","React"]' />
    </SimpleForm>
  </Edit>
);
