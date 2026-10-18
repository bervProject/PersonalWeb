import React from 'react';
import {
  List,
  Datagrid,
  TextField,
  DateField,
  ImageField,
  UrlField,
  BooleanField,
  NumberField,
  Create,
  SimpleForm,
  TextInput,
  BooleanInput,
  NumberInput,
  Edit,
  Show,
  SimpleShowLayout,
  EditButton,
  ShowButton
} from 'react-admin';

export const PortofolioList = (props: object) => (
  <List {...props}>
    <Datagrid>
      <TextField source="id" />
      <TextField source="title" />
      <TextField source="slug" />
      <BooleanField source="isPublished" />
      <NumberField source="orderIndex" />
      <EditButton />
      <ShowButton />
    </Datagrid>
  </List>
);

export const PortofolioShow = (props: object) => (
  <Show {...props}>
    <SimpleShowLayout>
      <TextField source="id" />
      <TextField source="title" />
      <TextField source="slug" />
      <TextField source="description" />
      <ImageField source="coverImage" />
      <UrlField source="link" />
      <UrlField source="repositoryLink" />
      <UrlField source="liveUrl" />
      <TextField source="techStack" />
      <BooleanField source="isPublished" />
      <NumberField source="orderIndex" />
    </SimpleShowLayout>
  </Show>
);

export const PortofolioCreate = (props: object) => (
  <Create {...props}>
    <SimpleForm>
      <TextInput source="title" />
      <TextInput source="slug" />
      <TextInput multiline source="description" />
      <TextInput source="coverImage" type="url" />
      <TextInput source="link" type="url" />
      <TextInput source="repositoryLink" type="url" />
      <TextInput source="liveUrl" type="url" />
      <TextInput source="techStack" helperText='JSON array e.g. ["TypeScript","React"]' />
      <BooleanInput source="isPublished" defaultValue={true} />
      <NumberInput source="orderIndex" defaultValue={0} />
    </SimpleForm>
  </Create>
);

export const PortofolioUpdate = (props: object) => (
  <Edit {...props}>
    <SimpleForm>
      <TextInput disabled source="id" />
      <TextInput source="title" />
      <TextInput source="slug" />
      <TextInput multiline source="description" />
      <TextInput source="coverImage" type="url" />
      <TextInput source="link" type="url" />
      <TextInput source="repositoryLink" type="url" />
      <TextInput source="liveUrl" type="url" />
      <TextInput source="techStack" helperText='JSON array e.g. ["TypeScript","React"]' />
      <BooleanInput source="isPublished" />
      <NumberInput source="orderIndex" />
    </SimpleForm>
  </Edit>
);
